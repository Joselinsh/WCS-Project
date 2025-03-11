import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

interface LoginResponse {
  message: string;
  token: string;
  roleId: number;
  designation: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5057/api/Auth';

  constructor(private http: HttpClient, private router: Router) {}
  login(loginDto: { email: string; password: string }): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, loginDto).pipe(
      tap(response => {
        console.log('Login response:', response); // Add this to debug the response
        localStorage.setItem('token', response.token);
        localStorage.setItem('roleId', response.roleId.toString());
        localStorage.setItem('designation', response.designation || '');

        const message = response.message?.toLowerCase() || '';
        console.log('Message:', message); // Add this to debug the message
        if (message.includes('admin dashboard')) {
          localStorage.setItem('role', 'Admin');
          this.router.navigate(['/admin/dashboard']);
        } else if (message.includes('employee dashboard')) {
          localStorage.setItem('role', 'Employee');
          if (response.designation?.toLowerCase().includes('manager')) {
            this.router.navigate(['/manager/dashboard']);
          } else {
            this.router.navigate(['/employee/home']);
          }
        } else if (message.includes('hr dashboard')) {
          localStorage.setItem('role', 'HR');
          this.router.navigate(['/hr/dashboard']);
        } 
        else if (message.includes('manager') && message.includes('dashboard')) { // Updated condition
          localStorage.setItem('role', 'Employee'); // Manager is a type of Employee
          this.router.navigate(['/manager/dashboard']);
        }
        else {
          console.log('No matching role found in message');
          this.router.navigate(['/']);
          localStorage.removeItem('token');
          localStorage.removeItem('role');
          localStorage.removeItem('roleId');
          localStorage.removeItem('designation');
          throw new Error('Role not assigned yet');
        }
      }),
      catchError(error => {
        console.error('Login failed:', error);
        localStorage.removeItem('token');
        localStorage.removeItem('role');
        localStorage.removeItem('roleId');
        localStorage.removeItem('designation');
        return of({ message: '', token: '', roleId: 0, designation: '' } as LoginResponse);
      })
    );
  }

  register(registerDto: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, registerDto);
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('roleId');
    localStorage.removeItem('role');
    localStorage.removeItem('designation');
    this.router.navigate(['/']);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  getRole(): string | null {
    return localStorage.getItem('role');
  }

  getRoleId(): number | null {
    const roleId = localStorage.getItem('roleId');
    return roleId ? Number(roleId) : null;
  }

  getDesignation(): string | null {
    return localStorage.getItem('designation');
  }

  getToken(): string | null {
    return localStorage.getItem('token'); // Added for consistency with EmployeeService
  }
}