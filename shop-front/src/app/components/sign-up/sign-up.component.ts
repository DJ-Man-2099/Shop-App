import { Component, OnInit } from '@angular/core';
import {
  EnhancedFormBuilderService,
  FormKeys,
} from '../../Services/enhanced-form-builder.service';
import { FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthenticationService } from '../../Services/authentication.service';
import {
  confirmPasswordValidator,
  passwordValidator,
  validatePasswordStrength,
} from '../../Validators/password-validators.service';
import { PasswordStrengthMeterComponent } from '../password-strength-meter/password-strength-meter.component';

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [ReactiveFormsModule, PasswordStrengthMeterComponent],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css',
})
export class SignUpComponent implements OnInit {
  static Path = 'signUp';

  NewUserForm!: FormGroup;
  NewUserFormControlNames!: FormKeys[];

  PasswordStrengthMeter: number = 0;
  PasswordStrengthMeterError: string = '';
  ConfirmPasswordError: string = '';

  constructor(
    private efb: EnhancedFormBuilderService,
    private authService: AuthenticationService
  ) {}

  ngOnInit(): void {
    const options = [
      { value: 'Admin', label: 'مدير' },
      { value: 'Worker', label: 'موظف' },
    ];

    this.efb.createForm({
      firstName: {
        type: 'text',
        displayName: 'الاسم الاول',
        controls: ['', Validators.required],
      },
      lastName: {
        type: 'text',
        displayName: 'الاسم الثاني',
        controls: ['', Validators.required],
      },
      userName: {
        type: 'text',
        displayName: 'اسم المستخدم',
        controls: ['', Validators.required],
      },
      password: {
        type: 'password',
        displayName: 'كلمة السر',
        controls: ['', [Validators.required, passwordValidator()]],
      },
      confirmPassword: {
        type: 'password',
        displayName: 'تأكيد كلمة السر',
        controls: ['', Validators.required],
      },
      role: {
        type: 'select',
        displayName: 'الوظيفة',
        options,
        controls: ['', Validators.required],
      },
    });

    this.NewUserForm = this.efb.resultForm;
    this.NewUserFormControlNames = this.efb.getFormControlNames();
    this.NewUserForm.get('password')?.valueChanges.subscribe((value) => {
      this.checkPasswordStrength(value);
    });
    this.NewUserForm.get('confirmPassword')?.valueChanges.subscribe((value) => {
      this.confirmPasswordsMatch();
    });
  }

  checkPasswordStrength(password: string) {
    const { strengthCount, errorMessage } = validatePasswordStrength(password);
    this.PasswordStrengthMeter = strengthCount;
    this.PasswordStrengthMeterError = errorMessage;
  }

  confirmPasswordsMatch() {
    const password = this.NewUserForm.get('password')?.value;
    const confirmPassword = this.NewUserForm.get('confirmPassword')?.value;
    this.ConfirmPasswordError =
      password !== confirmPassword ? 'كلمة السر غير متطابقة' : '';
    console.log(this.ConfirmPasswordError);
  }

  async onSubmit() {
    const submitFn =
      this.NewUserForm.get('role')?.value === 'Admin'
        ? this.authService.signUpAsAdmin.bind(this.authService)
        : this.authService.signUpAsWorker.bind(this.authService);
    var res = await submitFn(this.NewUserForm.value);
    if (res.ok) {
      alert('تم التسجيل بنجاح');
    } else {
      if (res.status === 400) {
        alert('اسم المستخدم موجود بالفعل');
      } else {
        alert('حدث خطأ ما');
      }
    }
  }
}
