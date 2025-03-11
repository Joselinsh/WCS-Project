import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { SuccessPopupComponent } from '../sucess-popup.component';// Adjust path as needed

@Injectable({
  providedIn: 'root'
})
export class SnackbarService {
  constructor(
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) {}

  showSuccess(message: string): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.data = { message: message || 'Timesheet Approved! 🎉' };
    dialogConfig.panelClass = 'success-popup-container';
    dialogConfig.disableClose = true; // Prevent closing by clicking outside
    dialogConfig.autoFocus = false;

    this.dialog.open(SuccessPopupComponent, dialogConfig);
  }

  showError(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 5000,
      panelClass: ['zoho-error'],
      verticalPosition: 'bottom',
      horizontalPosition: 'center'
    });
  }

  showWarning(message: string): void {
    this.snackBar.open(message, 'Close', {
      duration: 4000,
      panelClass: ['zoho-warning'],
      verticalPosition: 'bottom',
      horizontalPosition: 'center'
    });
  }
}