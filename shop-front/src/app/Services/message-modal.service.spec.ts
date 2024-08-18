import { TestBed } from '@angular/core/testing';

import { MessageModalService } from './message-modal.service';

describe('MessageModalService', () => {
  let service: MessageModalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MessageModalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
