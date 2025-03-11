import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Profile, Timesheet, LeaveRequest } from './app/models/employee.model';

interface PendingLeave {
  id: number;
  employeeName: string;
  startDate: string;
  endDate: string;
  reason: string;
  status: string;
  managerComments?: string;
  hrComments?: string;
}

interface PendingTimesheet {
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

interface PendingLeave {
  id: number;
  employeeName: string;
  startDate: string;
  endDate: string;
  reason: string;
  status: string;
  managerComments?: string; // Added for manager comments
  hrComments?: string;     // Added for HR comments (may be empty for pending)
}
 

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = 'http://localhost:5057/api';
  constructor(private http: HttpClient) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    console.log('Auth Token:', token);
    return new HttpHeaders({
      'Authorization': token ? `Bearer ${token}` : '',
      'Content-Type': 'application/json'
    });
  }
  getProfile(): Observable<Profile> {
    const url = `${this.apiUrl}/employee/Profile`;
    console.log('Fetching profile, URL:', url);
    return this.http.get<Profile>(url, { headers: this.getAuthHeaders() });
  }

  updateProfile(userId: number, profile: Profile): Observable<Profile> {
    const url = `${this.apiUrl}/employee/Profile/${userId}`;
    return this.http.put<Profile>(url, profile, { headers: this.getAuthHeaders() });
  }

  submitTimesheet(timesheet: Partial<Timesheet>): Observable<Timesheet> {
    const url = `${this.apiUrl}/employee/Timesheets/Submit`;
    return this.http.post<Timesheet>(url, timesheet, { headers: this.getAuthHeaders() });
  }

  updateTimesheet(timesheetId: number, timesheet: Partial<Timesheet>): Observable<Timesheet> {
    const url = `${this.apiUrl}/employee/Timesheets/Update/${timesheetId}`;
    return this.http.put<Timesheet>(url, timesheet, { headers: this.getAuthHeaders() });
  }

  deleteTimesheet(timesheetId: number): Observable<void> {
    const url = `${this.apiUrl}/employee/Timesheets/Delete/${timesheetId}`;
    return this.http.delete<void>(url, { headers: this.getAuthHeaders() });
  }

  requestLeave(leaveRequest: Partial<LeaveRequest>): Observable<LeaveRequest> {
    const url = `${this.apiUrl}/employee/Leaves/Submit`;
    return this.http.post<LeaveRequest>(url, leaveRequest, { headers: this.getAuthHeaders() });
  }

  getTimesheets(): Observable<Timesheet[]> {
    const url = `${this.apiUrl}/employee/timesheets`;
    console.log('Fetching timesheets, URL:', url);
    return this.http.get<Timesheet[]>(url, { headers: this.getAuthHeaders() });
  }

  getLeaves(): Observable<LeaveRequest[]> {
    const url = `${this.apiUrl}/employee/Leaves`;
    console.log('Fetching leaves, URL:', url);
    return this.http.get<LeaveRequest[]>(url, { headers: this.getAuthHeaders() });
  }

  getPendingLeaves(): Observable<PendingLeave[]> {
    const url = `${this.apiUrl}/approvals/leave/pending`;
    return this.http.get<PendingLeave[]>(url, { headers: this.getAuthHeaders() });
  }

  getPendingTimesheets(): Observable<PendingTimesheet[]> {
    const url = `${this.apiUrl}/approvals/timesheet/pending`;
    return this.http.get<PendingTimesheet[]>(url, { headers: this.getAuthHeaders() });
  }

  approveLeave(leaveId: number, isApproved: boolean): Observable<any> {
    const url = `${this.apiUrl}/approvals/leave-approval`;
    return this.http.post<any>(url, { leaveId, isApproved }, { headers: this.getAuthHeaders() });
  }

  approveTimesheet(timesheetId: number, isApproved: boolean, comments: string, isManager: boolean = true): Observable<any> {
    const url = `${this.apiUrl}/approvals/timesheet-approval`;
    const body = {
      timesheetId: timesheetId,
      isApproved: isApproved,
      [isManager ? 'managerComments' : 'hrComments']: comments || (isManager ? 'No comments provided' : 'No HR comments provided'),
      [isManager ? 'hrComments' : 'managerComments']: '' // Ensure the other comment field is empty
    };
    console.log('=== APPROVAL REQUEST ===');
    console.log('URL:', url);
    console.log('Payload:', JSON.stringify(body, null, 2));
    console.log('=== END REQUEST ===');
    return this.http.post<any>(url, body, { headers: this.getAuthHeaders() });
  }
}