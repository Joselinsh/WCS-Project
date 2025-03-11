import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerPendingLeavesComponent } from './manager-pending-leaves.component';

describe('ManagerPendingLeavesComponent', () => {
  let component: ManagerPendingLeavesComponent;
  let fixture: ComponentFixture<ManagerPendingLeavesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManagerPendingLeavesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManagerPendingLeavesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
