import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { EmployeeService } from '../../employee.service';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-hr-dashboard-content',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './hr-dashboard-content.component.html',
  styleUrls: ['./hr-dashboard-content.component.css']
})
export class HRDashboardContentComponent implements OnInit {
  pendingLeavesCount: number = 0;
  pendingTimesheetsCount: number = 0;
  totalApprovals: number = 0;
  isLoading: boolean = false;
  errorMessage: string | null = null;

  constructor(
    private employeeService: EmployeeService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    if (!this.authService.isLoggedIn()) {
      console.warn('User not logged in, redirecting to login');
      return;
    }
    this.fetchData();
  }

  fetchData(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.employeeService.getPendingLeaves().subscribe({
      next: (leaves) => {
        this.pendingLeavesCount = leaves.filter(l => l.status === 'ManagerApproved').length;
        this.fetchPendingTimesheets();
      },
      error: (err) => {
        console.error('Failed to fetch pending leaves:', err);
        this.errorMessage = 'Failed to load pending leaves';
        this.isLoading = false;
      },
    });
  }

  fetchPendingTimesheets(): void {
    this.employeeService.getPendingTimesheets().subscribe({
      next: (timesheets) => {
        this.pendingTimesheetsCount = timesheets.filter(t => t.status === 'ManagerApproved').length;
        this.totalApprovals = this.pendingLeavesCount + this.pendingTimesheetsCount;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to fetch pending timesheets:', err);
        this.errorMessage = 'Failed to load pending timesheets';
        this.isLoading = false;
      },
    });
  }
}