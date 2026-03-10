import { TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';

import { InseeIndexListComponent } from './insee-index-list.component';
import { InseeIndexService } from '../Services/insee-index.service';
import { InseeIndex } from '../models/insee-index';

const MOCK_INDEXES: InseeIndex[] = [
  { period: '2024-01', irl: 100, ilc: 200, icc: 300, ilat: 400 },
  { period: '2023/12', irl: 95,  ilc: 195, icc: 295, ilat: 395 },
];

describe('InseeIndexListComponent', () => {
  let serviceSpy: jasmine.SpyObj<InseeIndexService>;

  beforeEach(async () => {
    serviceSpy = jasmine.createSpyObj('InseeIndexService', ['getInseeIndexes']);
    serviceSpy.getInseeIndexes.and.returnValue(of(MOCK_INDEXES));

    await TestBed.configureTestingModule({
      imports: [InseeIndexListComponent],
      providers: [{ provide: InseeIndexService, useValue: serviceSpy }],
    }).compileComponents();
  });

  it('should create', () => {
    expect(TestBed.createComponent(InseeIndexListComponent).componentInstance).toBeTruthy();
  });

  it('should call getInseeIndexes on ngOnInit', () => {
    TestBed.createComponent(InseeIndexListComponent).detectChanges();
    expect(serviceSpy.getInseeIndexes).toHaveBeenCalledTimes(1);
  });

  it('should populate indexes from service', () => {
    const fixture = TestBed.createComponent(InseeIndexListComponent);
    fixture.detectChanges();
    expect(fixture.componentInstance.indexes).toEqual(MOCK_INDEXES);
  });

  it('should set errorMessage and leave indexes empty on error', () => {
    serviceSpy.getInseeIndexes.and.returnValue(throwError(() => ({ status: 500 })));
    const fixture = TestBed.createComponent(InseeIndexListComponent);
    fixture.detectChanges();
    expect(fixture.componentInstance.indexes).toEqual([]);
    expect(fixture.componentInstance.errorMessage).toContain('500');
  });

  describe('getPeriodYear', () => {
    let comp: InseeIndexListComponent;
    beforeEach(() => { comp = TestBed.createComponent(InseeIndexListComponent).componentInstance; });

    it('returns the year part of a YYYY-MM string', () => {
      expect(comp.getPeriodYear('2024-03')).toBe('2024');
    });

    it('handles "/" separator', () => {
      expect(comp.getPeriodYear('2023/06')).toBe('2023');
    });

    it('returns empty string for empty input', () => {
      expect(comp.getPeriodYear('')).toBe('');
    });
  });

  describe('getPeriodMonth', () => {
    let comp: InseeIndexListComponent;
    beforeEach(() => { comp = TestBed.createComponent(InseeIndexListComponent).componentInstance; });

    it('returns JAN for month 1', () => { expect(comp.getPeriodMonth('2024-01')).toBe('JAN'); });
    it('returns JUN for month 6', () => { expect(comp.getPeriodMonth('2024-06')).toBe('JUN'); });
    it('returns DEC for month 12', () => { expect(comp.getPeriodMonth('2024-12')).toBe('DEC'); });
    it('handles "/" separator',     () => { expect(comp.getPeriodMonth('2023/09')).toBe('SEP'); });

    it('returns empty string for empty input', () => {
      expect(comp.getPeriodMonth('')).toBe('');
    });

    it('returns raw part for out-of-range month number', () => {
      expect(comp.getPeriodMonth('2024-13')).toBe('13');
    });
  });
});
