import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';

import { RealEstateService } from './real-estate.service';

describe('RealEstateService', () => {
  let service: RealEstateService;

  beforeEach(() => {
    TestBed.configureTestingModule({ providers: [provideHttpClient()] });
    service = TestBed.inject(RealEstateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
