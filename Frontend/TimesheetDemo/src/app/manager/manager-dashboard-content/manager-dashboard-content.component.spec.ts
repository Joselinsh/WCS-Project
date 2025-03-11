import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerDashboardContentComponent } from './manager-dashboard-content.component';

describe('ManagerDashboardContentComponent', () => {
  let component: ManagerDashboardContentComponent;
  let fixture: ComponentFixture<ManagerDashboardContentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManagerDashboardContentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManagerDashboardContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
