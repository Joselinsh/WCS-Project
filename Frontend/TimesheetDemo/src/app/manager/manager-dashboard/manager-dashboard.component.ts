import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../auth.service';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-manager-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule
  ],
  templateUrl: './manager-dashboard.component.html',
  styleUrls: ['./manager-dashboard.component.css']
})
export class ManagerDashboardComponent implements OnInit {
  userInitials: string = '';
  darkMode: boolean = false;
  showDropdown: boolean = false;
  isLoading: boolean = false;
  companyName: string = 'Pluto Zen'; 
  constructor(
    private authService: AuthService,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const role = this.authService.getRole();
    const designation = this.authService.getDesignation();

    if (role !== 'Employee' || !designation?.toLowerCase().includes('manager')) {
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

    this.userService.userInitials$.subscribe(initials => {
      this.userInitials = initials || this.getInitials('Manager'); 
    });

    this.darkMode = localStorage.getItem('darkMode') === 'true';
    this.updateTheme();
  }

  toggleDarkMode(): void {
    this.darkMode = !this.darkMode;
    localStorage.setItem('darkMode', this.darkMode.toString());
    this.updateTheme();
  }

  updateTheme(): void {
    document.documentElement.classList.toggle('dark', this.darkMode);
  }

  toggleDropdown(): void {
    this.showDropdown = !this.showDropdown;
  }

  logout(): void {
    this.authService.logout();
    this.isLoading = true;
    this.showDropdown = false;
    setTimeout(() => {
      this.isLoading = false;
      this.router.navigate(['/']);
    }, 1000);
  }

  private getInitials(fullName: string): string {
    return fullName
      .split(' ')
      .map(name => name.charAt(0))
      .join('')
      .slice(0, 2) || 'M';
  }
}