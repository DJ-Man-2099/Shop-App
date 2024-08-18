import {
  ValidatorFn,
  AbstractControl,
  ValidationErrors,
  FormGroup,
} from '@angular/forms';

export function passwordValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;
    if (!value) {
      return null;
    }
    const { passwordValid } = validatePasswordStrength(value);
    return !passwordValid ? { passwordStrength: true } : null;
  };
}

export function validatePasswordStrength(value: any) {
  let strengthCount = 0;
  let errorMessage = '';
  if (!value) {
    return { passwordValid: false, strengthCount, errorMessage };
  }
  const hasLowerCase = /[a-z]/.test(value);
  if (!!hasLowerCase) {
    strengthCount++;
  } else if (!errorMessage) {
    errorMessage = 'كلمة السر لا تحتوي على Lowercase Letter';
  }
  const hasUpperCase = /[A-Z]/.test(value);
  if (!!hasUpperCase) {
    strengthCount++;
  } else if (!errorMessage) {
    errorMessage = 'كلمة السر لا تحتوي على Uppercase Letter';
  }
  const hasNumeric = /[0-9]/.test(value);
  if (!!hasNumeric) {
    strengthCount++;
  } else if (!errorMessage) {
    errorMessage = 'كلمة السر لا تحتوي على رقم';
  }
  const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(value);
  if (!!hasSpecialChar) {
    strengthCount++;
  } else if (!errorMessage) {
    errorMessage = 'كلمة السر لا تحتوي على رموز';
  }
  const isValidLength = value.length >= 8;
  if (!!isValidLength) {
    strengthCount++;
  } else if (!errorMessage) {
    errorMessage = 'كلمة السر قصيرة';
  }
  const passwordValid =
    hasUpperCase &&
    hasLowerCase &&
    hasNumeric &&
    hasSpecialChar &&
    isValidLength;
  return { passwordValid, strengthCount, errorMessage };
}

export function confirmPasswordValidator(
  passwordControlName: string,
  confirmPasswordControlName: string
): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const formGroup = control as FormGroup;
    const passwordControl = formGroup.controls[passwordControlName];
    const confirmPasswordControl =
      formGroup.controls[confirmPasswordControlName];

    if (!passwordControl || !confirmPasswordControl) {
      return null;
    }

    if (
      confirmPasswordControl.errors &&
      !confirmPasswordControl.errors['passwordMismatch']
    ) {
      return null;
    }

    if (passwordControl.value !== confirmPasswordControl.value) {
      confirmPasswordControl.setErrors({ passwordMismatch: true });
    } else {
      confirmPasswordControl.setErrors(null);
    }

    return null;
  };
}
