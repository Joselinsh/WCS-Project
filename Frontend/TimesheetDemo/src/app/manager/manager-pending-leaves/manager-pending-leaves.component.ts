import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../../../employee.service';
import { SnackbarService } from '../../services/snackbar.services';
import { CommonModule } from '@angular/common';
import { NgChartsModule } from 'ng2-charts';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog.component';
import { AuthService } from '../../auth.service';

import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-manager-pending-leaves',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatDialogModule,
    // ConfirmDialogComponent
   
  ],
  templateUrl: './manager-pending-leaves.component.html',
  styleUrls: ['./manager-pending-leaves.component.css']
})
export class ManagerPendingLeavesComponent implements OnInit {
  displayedColumns: string[] = ['employeeName', 'startDate', 'endDate', 'reason', 'managerComments', 'hrComments', 'actions'];
  pendingLeaves: any[] = [];

  constructor(
    private employeeService: EmployeeService,
    private snackbarService: SnackbarService,
    private dialog: MatDialog,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const role = this.authService.getRole();
    const designation = this.authService.getDesignation();

    if (role !== 'Employee' || !designation?.includes('Manager')) {
      if (role === 'Admin') {
        this.router.navigate(['/admin/dashboard']);
      } else if (role === 'HR') {
        this.router.navigate(['/hr/dashboard']);
      } else if (role === 'Employee') {
        this.router.navigate(['/employee/home']);
      } else {
        this.router.navigate(['/']);
      }
      return;
    }

    this.loadPendingLeaves();
  }

  loadPendingLeaves(): void {
    this.employeeService.getPendingLeaves().subscribe({
      next: (leaves) => {
        this.pendingLeaves = leaves;
      },
      error: (err) => {
        this.snackbarService.showError('Error fetching pending leaves');
        console.error(err);
      }
    });
  }

  approveLeave(leaveId: number, isApproved: boolean): void {
    const action = isApproved ? 'approve' : 'reject';
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { message: `Are you sure you want to ${action} this leave request?` }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.employeeService.approveLeave(leaveId, isApproved).subscribe({
          next: (response) => {
            this.snackbarService.showSuccess(response.message);
            this.loadPendingLeaves();
          },
          error: (err) => {
            this.snackbarService.showError('Error processing leave request');
            console.error(err);
          }
        });
      }
    });
  }
}
