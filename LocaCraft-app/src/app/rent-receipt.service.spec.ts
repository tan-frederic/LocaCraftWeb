import { TestBed } from '@angular/core/testing';

import { RentReceiptService } from './Services/rent-receipt.service';

describe('RentReceiptService', () => {
  let service: RentReceiptService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RentReceiptService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
