import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { EmployeeService } from '../../employee.service';
import { SnackbarService } from '../services/snackbar.services';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../shared/confirm-dialog.component';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

interface Leave {
  id: number;
  employeeName: string;
  startDate: string;
  endDate: string;
  reason: string;
  status: string;
}

@Component({
  selector: 'app-hr-pending-leaves',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatDialogModule, ConfirmDialogComponent],
  templateUrl: './hr-pending-leaves.component.html',
  styleUrls: ['./hr-pending-leaves.component.css']
})
export class HRPendingLeavesComponent implements OnInit {
  displayedColumns: string[] = ['employeeName', 'startDate', 'endDate', 'reason', 'status', 'actions'];
  pendingLeaves: Leave[] = [];

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

    this.loadPendingLeaves();
  }

  loadPendingLeaves(): void {
    this.employeeService.getPendingLeaves().subscribe({
      next: (leaves) => {
        this.pendingLeaves = leaves.filter(l => l.status === 'ManagerApproved');
      },
      error: (err) => {
        this.snackbarService.showError('Error fetching pending leaves');
        console.error(err);
      }
    });
  }

  approveLeave(leaveId: number): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { message: 'Are you sure you want to approve this leave request?' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const roleId = this.authService.getRoleId();
        if (!roleId) {
          this.snackbarService.showError('Approver ID not found');
          return;
        }
        this.employeeService.approveLeave(leaveId, true).subscribe({
          next: (response) => {
            this.snackbarService.showSuccess(response.message || 'Leave approved successfully');
            this.loadPendingLeaves();
          },
          error: (err) => {
            this.snackbarService.showError('Error approving leave');
            console.error(err);
          }
        });
      }
    });
  }

  rejectLeave(leaveId: number): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { message: 'Are you sure you want to reject this leave request? Enter reason:', input: true }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const roleId = this.authService.getRoleId();
        if (!roleId) {
          this.snackbarService.showError('Approver ID not found');
          return;
        }
        const comment = result.input || 'HR-Rejected';
        this.employeeService.approveLeave(leaveId, false).subscribe({
          next: (response) => {
            this.snackbarService.showSuccess(response.message || 'Leave rejected successfully');
            this.loadPendingLeaves();
          },
          error: (err) => {
            this.snackbarService.showError('Error rejecting leave');
            console.error(err);
          }
        });
      }
    });
  }
}