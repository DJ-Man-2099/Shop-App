import { TestBed } from '@angular/core/testing';

import { ModalNavigateService } from './modal-navigate.service';

describe('ModalNavigateService', () => {
  let service: ModalNavigateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ModalNavigateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
