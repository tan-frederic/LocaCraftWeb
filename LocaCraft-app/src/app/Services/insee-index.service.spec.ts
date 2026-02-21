import { TestBed } from '@angular/core/testing';

import { InseeIndexService } from './insee-index.service';

describe('InseeIndexService', () => {
  let service: InseeIndexService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InseeIndexService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
