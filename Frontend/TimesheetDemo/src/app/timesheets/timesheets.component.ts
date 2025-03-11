import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EmployeeService } from '../../employee.service';
import { Timesheet, Profile } from '../models/employee.model';
import { HttpErrorResponse } from '@angular/common/http';
import { trigger, transition, style, animate } from '@angular/animations';

@Component({
  selector: 'app-timesheets',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './timesheets.component.html',
  styleUrls: ['./timesheets.component.css'],
  animations: [
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('500ms', style({ opacity: 1 }))
      ])
    ]),
    trigger('cardAppear', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(20px)' }),
        animate('300ms ease-out')
      ])
    ]),
    trigger('slideInOut', [
      transition(':enter', [
        style({ height: '0', opacity: 0 }),
        animate('300ms ease-in', style({ height: '*', opacity: 1 }))
      ]),
      transition(':leave', [
        animate('300ms ease-out', style({ height: '0', opacity: 0 }))
      ])
    ])
  ]
})
export class TimesheetsComponent implements OnInit {
  timesheets: Timesheet[] = [];
  profile: Profile | null = null;
  newTimesheet: Partial<Timesheet> = {
    projectName: '',
    date: '',
    hoursWorked: 0,
    description: '',
  };
  isLoading = false;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  editingTimesheet: Timesheet | null = null;
  approvingTimesheet: Timesheet | null = null;
  approvalComments: string = '';
  showTimesheets: boolean = false; 

  constructor(private employeeService: EmployeeService) {}

  ngOnInit(): void {
    this.fetchProfile();
    this.fetchTimesheets();
  }

  fetchProfile(): void {
    this.employeeService.getProfile().subscribe({
      next: (response) => {
        this.profile = response;
      },
      error: (err: HttpErrorResponse) => {
        console.error('Failed to fetch profile:', err);
        this.errorMessage = `Failed to load profile: ${err.status} - ${err.statusText || err.message}`;
      },
    });
  }

  fetchTimesheets(): void {
    this.isLoading = true;
    this.errorMessage = null;
    this.successMessage = null;
    this.employeeService.getTimesheets().subscribe({
      next: (response) => {
        this.timesheets = response;
        this.isLoading = false;
      },
      error: (err: HttpErrorResponse) => {
        console.error('Failed to fetch timesheets:', err);
        this.errorMessage = `Failed to load timesheets: ${err.status} - ${err.statusText || err.message}`;
        this.isLoading = false;
      },
    });
  }

  submitTimesheet(): void {
    if (!this.newTimesheet.projectName || !this.newTimesheet.date || !this.newTimesheet.hoursWorked) {
      this.errorMessage = 'Please fill in all required fields.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;
    this.successMessage = null;

    this.employeeService.submitTimesheet(this.newTimesheet).subscribe({
      next: (response) => {
        this.timesheets.push(response);
        this.newTimesheet = { projectName: '', date: '', hoursWorked: 0, description: '' };
        this.successMessage = 'Timesheet submitted successfully!';
        this.isLoading = false;
      },
      error: (err: HttpErrorResponse) => {
        console.error('Failed to submit timesheet:', err);
        this.errorMessage = `Failed to submit timesheet: ${err.status} - ${err.statusText || err.message}`;
        this.isLoading = false;
      },
    });
  }

  startEditing(timesheet: Timesheet): void {
    if (timesheet.status !== 'Pending') {
      this.errorMessage = 'Only pending timesheets can be edited.';
      return;
    }
    this.editingTimesheet = { ...timesheet };
  }

  updateTimesheet(): void {
    if (!this.editingTimesheet) return;

    this.isLoading = true;
    this.errorMessage = null;
    this.successMessage = null;

    this.employeeService.updateTimesheet(this.editingTimesheet.timesheetId, this.editingTimesheet).subscribe({
      next: (response) => {
        const index = this.timesheets.findIndex(t => t.timesheetId === this.editingTimesheet!.timesheetId);
        if (index !== -1) this.timesheets[index] = response;
        this.editingTimesheet = null;
        this.successMessage = 'Timesheet updated successfully!';
        this.isLoading = false;
      },
      error: (err: HttpErrorResponse) => {
        console.error('Failed to update timesheet:', err);
        this.errorMessage = `Failed to update timesheet: ${err.status} - ${err.statusText || err.message}`;
        this.isLoading = false;
      },
    });
  }

  deleteTimesheet(timesheetId: number): void {
    const timesheet = this.timesheets.find(t => t.timesheetId === timesheetId);
    if (!timesheet || timesheet.status !== 'Pending') {
      this.errorMessage = 'Only pending timesheets can be deleted.';
      return;
    }

    if (!confirm('Are you sure you want to delete this timesheet?')) return;

    this.isLoading = true;
    this.errorMessage = null;
    this.successMessage = null;

    this.employeeService.deleteTimesheet(timesheetId).subscribe({
      next: () => {
        this.timesheets = this.timesheets.filter(t => t.timesheetId !== timesheetId);
        this.successMessage = 'Timesheet deleted successfully!';
        this.isLoading = false;
      },
      error: (err: HttpErrorResponse) => {
        console.error('Failed to delete timesheet:', err);
        this.errorMessage = `Failed to delete timesheet: ${err.status} - ${err.statusText || err.message}`;
        this.isLoading = false;
      },
    });
  }

  cancelEdit(): void {
    this.editingTimesheet = null;
    this.errorMessage = null;
  }

  startApproval(timesheet: Timesheet): void {
    if (timesheet.status !== 'Pending' && timesheet.status !== 'ManagerApproved') {
      this.errorMessage = 'Only pending or manager-approved timesheets can be approved.';
      return;
    }
    // Restrict to Manager or HR
    if (this.role !== 'Manager' && this.role !== 'HR') {
      this.errorMessage = 'Only Managers or HR can approve/reject timesheets.';
      return;
    }
    this.approvingTimesheet = { ...timesheet };
    this.approvalComments = '';
  }

  approveTimesheet(isApproved: boolean): void {
    if (!this.approvingTimesheet) return;

    this.isLoading = true;
    this.errorMessage = null;
    this.successMessage = null;

    this.employeeService.approveTimesheet(this.approvingTimesheet.timesheetId, isApproved, this.approvalComments).subscribe({
      next: (response) => {
        const index = this.timesheets.findIndex(t => t.timesheetId === this.approvingTimesheet!.timesheetId);
        if (index !== -1) {
          this.timesheets[index] = { ...this.timesheets[index], status: response.status, managerComments: response.managerComments, hrComments: response.hrComments };
        }
        this.approvingTimesheet = null;
        this.successMessage = response.message || 'Timesheet processed successfully!';
        this.isLoading = false;
      },
      error: (err: HttpErrorResponse) => {
        console.error('Failed to approve timesheet:', err);
        this.errorMessage = `Failed to approve timesheet: ${err.status} - ${err.statusText || err.message}`;
        this.isLoading = false;
      },
    });
  }

  cancelApproval(): void {
    this.approvingTimesheet = null;
    this.approvalComments = '';
    this.errorMessage = null;
  }

  toggleTimesheets(): void {
    this.showTimesheets = !this.showTimesheets;
  }

  get role(): string | null {
    return localStorage.getItem('role');
  }
}