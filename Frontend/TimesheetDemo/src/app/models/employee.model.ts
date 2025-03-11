export interface Timesheet {
  timesheetId: number;
  projectName: string;
  date: string;
  hoursWorked: number;
  description: string;
  status: string; // camelCase to match backend
}

export interface Profile {
  employeeId: number;
  fullName: string;
  email: string;
  department: string;
  designation: string;
  timesheets: Timesheet[];
}

export interface LeaveRequest {
  id: number;
  startDate: string;
  endDate: string;
  status: string; // Changed to camelCase to match backend response (e.g., "Pending", "Approved", "Rejected")
}

export interface Timesheet {
  timesheetId: number;
  projectName: string;
  date: string;
  hoursWorked: number;
  description: string;
  status: string;
  managerComments?: string;
  hrComments?: string;
}

export interface Leave {
  id: number;
  employeeName: string;
  startDate: string;
  endDate: string;
  reason: string;
  status: string;
  managerComments?: string; // Added for manager comments
  hrComments?: string;     // Added for HR comments (may be empty for pending)
}   