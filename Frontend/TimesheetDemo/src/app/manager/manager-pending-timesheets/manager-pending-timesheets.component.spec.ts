import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerPendingTimesheetsComponent } from './manager-pending-timesheets.component';

describe('ManagerPendingTimesheetsComponent', () => {
  let component: ManagerPendingTimesheetsComponent;
  let fixture: ComponentFixture<ManagerPendingTimesheetsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManagerPendingTimesheetsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManagerPendingTimesheetsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
