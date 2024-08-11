import { TestBed } from '@angular/core/testing';

import { RedirectToLoginService } from './redirect-to-login.service';

describe('RedirectToLoginService', () => {
  let service: RedirectToLoginService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RedirectToLoginService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
