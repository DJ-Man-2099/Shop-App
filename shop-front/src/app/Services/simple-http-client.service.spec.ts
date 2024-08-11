import { TestBed } from '@angular/core/testing';

import { SimpleHttpClientService } from './simple-http-client.service';

describe('SimpleHttpClientService', () => {
  let service: SimpleHttpClientService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SimpleHttpClientService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
