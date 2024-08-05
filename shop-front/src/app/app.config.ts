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

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideClientHydration(),
    { provide: HTTP_INTERCEPTORS, useClass: HttpBaseAdderService, multi: true },
    provideHttpClient(withInterceptorsFromDi(), withFetch()),
    importProvidersFrom(FormsModule, ReactiveFormsModule), // Use importProvidersFrom to include FormsModule and ReactiveFormsModule
  ],
};
