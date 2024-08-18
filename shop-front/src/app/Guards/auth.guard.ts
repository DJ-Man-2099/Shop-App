import { Inject, Injectable, InjectionToken } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';
import { PLATFORM_ID } from '@angular/core';
import { AuthenticationService } from '../Services/authentication.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private authService: AuthenticationService,
    @Inject(PLATFORM_ID) private platformId: InjectionToken<object>
  ) {}

  canActivate(): boolean {
    if (isPlatformBrowser(this.platformId)) {
      // Implement your authentication check logic here
      const isAuthenticated = this.authService.isAuthenticated();
      if (isAuthenticated) {
        return true;
      } else {
        this.router.navigate(['/login']);
        return false;
      }
    }
    return false;
  }
}
