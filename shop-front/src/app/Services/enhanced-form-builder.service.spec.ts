import { TestBed } from '@angular/core/testing';

import { EnhancedFormBuilderService } from './enhanced-form-builder.service';

describe('EnhancedFormBuilderService', () => {
  let service: EnhancedFormBuilderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EnhancedFormBuilderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
