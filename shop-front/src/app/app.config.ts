import {
  ApplicationConfig,
  importProvidersFrom,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'; // Import provideForms
import { provideClientHydration } from '@angular/platform-browser';
import {
  HTTP_INTERCEPTORS,
  provideHttpClient,
  withFetch,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { HttpBaseAdderService } from './MiddleWare/http-base-adder.service';
import { routes } from './app.routes';
import { AuthCookieAdderService } from './MiddleWare/auth-cookie-adder.service';
import { RedirectToLoginService } from './MiddleWare/redirect-to-login.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideClientHydration(),
    provideHttpClient(withInterceptorsFromDi(), withFetch()),
    importProvidersFrom(FormsModule, ReactiveFormsModule), // Use importProvidersFrom to include FormsModule and ReactiveFormsModule
    { provide: HTTP_INTERCEPTORS, useClass: HttpBaseAdderService, multi: true },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthCookieAdderService,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: RedirectToLoginService,
      multi: true,
    },
  ],
};
