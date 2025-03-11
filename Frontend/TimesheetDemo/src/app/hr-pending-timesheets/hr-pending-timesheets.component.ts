import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { EmployeeService } from '../../employee.service';
import { SnackbarService } from '../services/snackbar.services';
import { ConfirmDialogComponent } from '../shared/confirm-dialog.component';
import { SuccessPopupComponent } from '../sucess-popup.component';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

interface Timesheet {
  timesheetId: number;
  employeeName: string;
  projectName: string;
  date: string;
  hoursWorked: number;
  description: string;
  status: string;
  managerComments?: string;
  hrComments?: string;
}

@Component({
  selector: 'app-hr-pending-timesheets',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatDialogModule,
    ConfirmDialogComponent,
    SuccessPopupComponent
  ],
  templateUrl: './hr-pending-timesheets.component.html',
  styleUrls: ['./hr-pending-timesheets.component.css']
})
export class HRPendingTimesheetsComponent implements OnInit {
  displayedColumns: string[] = ['employeeName', 'projectName', 'date', 'hoursWorked', 'description', 'managerComments', 'hrComments', 'status', 'actions'];
  pendingTimesheets: Timesheet[] = [];

  constructor(
    private employeeService: EmployeeService,
    private snackbarService: SnackbarService,
    private dialog: MatDialog,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const role = this.authService.getRole();
    if (role !== 'HR') {
      if (role === 'Admin') {
        this.router.navigate(['/admin/dashboard']);
      } else if (role === 'Employee' && this.authService.getDesignation()?.toLowerCase().includes('manager')) {
        this.router.navigate(['/manager/dashboard']);
      } else if (role === 'Employee') {
        this.router.navigate(['/employee/home']);
      } else {
        this.router.navigate(['/']);
      }
      return;
    }

    this.loadPendingTimesheets();
  }

  loadPendingTimesheets(): void {
    this.employeeService.getPendingTimesheets().subscribe({
      next: (timesheets) => {
        this.pendingTimesheets = timesheets.filter(t => t.status === 'ManagerApproved');
      },
      error: (err) => {
        this.snackbarService.showError('Error fetching pending timesheets');
        console.error('Error fetching timesheets:', err);
      }
    });
  }

  approveTimesheet(timesheetId: number, isApproved: boolean): void {
    const action = isApproved ? 'approve' : 'reject';
    const dialogConfig: MatDialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      message: `Are you sure you want to ${action} this timesheet?`,
      showComments: true,
      comments: ''
    };
    dialogConfig.panelClass = 'custom-dialog-container';
    dialogConfig.autoFocus = true;

    const dialogRef = this.dialog.open(ConfirmDialogComponent, dialogConfig);

    dialogRef.afterOpened().subscribe(() => {
      const dialogElement = document.querySelector('.custom-dialog-container.cdk-overlay-pane') as HTMLElement;
      if (dialogElement) {
        dialogElement.style.position = 'fixed';
        dialogElement.style.top = '5vh';
        dialogElement.style.left = '50%';
        dialogElement.style.transform = 'translateX(-50%)';
        dialogElement.style.zIndex = '1500';
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.confirmed) {
        const comments = result.comments || (isApproved ? 'No HR comments provided' : 'HR-Rejected');
        const roleId = this.authService.getRoleId();
        if (!roleId) {
          this.snackbarService.showError('Approver ID not found');
          return;
        }
        console.log('=== HR USER ACTION ===');
        console.log('Processing timesheet with:', { timesheetId, isApproved, comments });
        console.log('=== END HR USER ACTION ===');
        this.employeeService.approveTimesheet(timesheetId, isApproved, comments, false).subscribe({
          next: (response) => {
            console.log('=== SUCCESS RESPONSE ===');
            console.log('Response:', response);
            console.log('Triggering success popup with message:', response?.message || `Timesheet ${timesheetId} ${isApproved ? 'Approved' : 'Rejected'} by HR! 🎉`);
            console.log('=== END SUCCESS RESPONSE ===');
            const successMessage = response?.message || `Timesheet ${timesheetId} ${isApproved ? 'Approved' : 'Rejected'} by HR! 🎉`;
            this.snackbarService.showSuccess(successMessage);
            this.loadPendingTimesheets();
          },
          error: (err: HttpErrorResponse) => {
            console.log('=== ERROR RESPONSE ===');
            console.log('HTTP Error:', err);
            console.log('Status:', err.status);
            console.log('Error Details:', JSON.stringify(err.error, null, 2));
            if (err.error && err.error.errors) {
              console.log('Validation Errors:', JSON.stringify(err.error.errors, null, 2));
            }
            console.log('=== END ERROR RESPONSE ===');
            this.snackbarService.showError(`Error ${isApproved ? 'approving' : 'rejecting'} timesheet`);
          }
        });
      }
    });
  }
}