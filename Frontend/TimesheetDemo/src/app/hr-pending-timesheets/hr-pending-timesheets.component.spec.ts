import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrPendingTimesheetsComponent } from './hr-pending-timesheets.component';

describe('HrPendingTimesheetsComponent', () => {
  let component: HrPendingTimesheetsComponent;
  let fixture: ComponentFixture<HrPendingTimesheetsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HrPendingTimesheetsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HrPendingTimesheetsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
