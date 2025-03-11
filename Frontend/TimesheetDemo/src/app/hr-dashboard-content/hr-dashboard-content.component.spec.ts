import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrDashboardContentComponent } from './hr-dashboard-content.component';

describe('HrDashboardContentComponent', () => {
  let component: HrDashboardContentComponent;
  let fixture: ComponentFixture<HrDashboardContentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HrDashboardContentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HrDashboardContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
