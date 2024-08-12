import { Component, OnInit } from '@angular/core';
import {
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  EnhancedFormBuilderService,
  FormKeys,
} from '../../Services/enhanced-form-builder.service';
import { AuthenticationService } from '../../Services/authentication.service';
import { Router } from '@angular/router';
import { MainPageComponent } from '../main-page/main-page.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent implements OnInit {
  static Path = 'login';

  loginForm!: FormGroup;
  loginFormControlNames!: FormKeys[];
  constructor(
    private efb: EnhancedFormBuilderService,
    private authService: AuthenticationService,
    private router: Router
  ) {}

  ngOnInit() {
    this.efb.createForm({
      userName: {
        type: 'text',
        displayName: 'اسم المستخدم',
        controls: ['', Validators.required],
      },
      password: {
        type: 'password',
        displayName: 'كلمة السر',
        controls: ['', Validators.required],
      },
    });
    this.loginForm = this.efb.resultForm;
    this.loginFormControlNames = this.efb.getFormControlNames();
  }

  async onSubmit() {
    var res = await this.authService.login(this.loginForm.value);
    if (res.ok) {
      this.router.navigate([MainPageComponent.Path]);
    }
  }
}
