import { TestBed } from '@angular/core/testing';

import { HttpBaseAdderService } from './http-base-adder.service';

describe('HttpBaseAdderService', () => {
  let service: HttpBaseAdderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HttpBaseAdderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
