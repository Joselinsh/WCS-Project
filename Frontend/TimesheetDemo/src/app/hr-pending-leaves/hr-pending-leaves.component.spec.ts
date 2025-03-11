import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrPendingLeavesComponent } from './hr-pending-leaves.component';

describe('HrPendingLeavesComponent', () => {
  let component: HrPendingLeavesComponent;
  let fixture: ComponentFixture<HrPendingLeavesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HrPendingLeavesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HrPendingLeavesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
