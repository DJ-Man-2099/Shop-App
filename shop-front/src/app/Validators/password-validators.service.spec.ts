import { TestBed } from '@angular/core/testing';

import { PasswordValidatorsService } from './password-validators.service';

describe('PasswordValidatorsService', () => {
  let service: PasswordValidatorsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PasswordValidatorsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
