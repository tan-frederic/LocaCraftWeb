import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';

import { InseeIndexService } from './insee-index.service';

describe('InseeIndexService', () => {
  let service: InseeIndexService;

  beforeEach(() => {
    TestBed.configureTestingModule({ providers: [provideHttpClient()] });
    service = TestBed.inject(InseeIndexService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
