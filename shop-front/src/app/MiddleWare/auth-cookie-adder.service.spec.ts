import { TestBed } from '@angular/core/testing';

import { AuthCookieAdderService } from './auth-cookie-adder.service';

describe('AuthCookieAdderService', () => {
  let service: AuthCookieAdderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthCookieAdderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
