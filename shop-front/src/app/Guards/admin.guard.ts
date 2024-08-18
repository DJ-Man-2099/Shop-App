import { inject, PLATFORM_ID } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthenticationService } from '../Services/authentication.service';
import { isPlatformBrowser } from '@angular/common';

export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthenticationService);
  const router = inject(Router);
  if (isPlatformBrowser(inject(PLATFORM_ID))) {
    const isAuthenticated = authService.isAuthenticated();
    if (isAuthenticated) {
      return authService.user?.role === 'Admin';
    } else {
      return false;
    }
  } else {
    return false;
  }
};
