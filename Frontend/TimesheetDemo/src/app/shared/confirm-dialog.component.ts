import { Component, Inject } from '@angular/core';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatInputModule,
    FormsModule
  ],
  template: `
    <div class="dialog-container">
      <h2 mat-dialog-title class="dialog-title">Confirm Action</h2>
      <mat-dialog-content class="dialog-content">
        <p class="dialog-message">{{ data.message }}</p>
        <ng-container *ngIf="data.showComments">
          <mat-form-field appearance="fill" class="w-full comment-field">
            <mat-label>Comments</mat-label>
            <textarea matInput [(ngModel)]="data.comments" placeholder="Enter comments (optional)" rows="4" cdkTextareaAutosize></textarea>
          </mat-form-field>
        </ng-container>
      </mat-dialog-content>
      <mat-dialog-actions class="dialog-actions">
        <button mat-button class="cancel-button" (click)="onNoClick()">Cancel</button>
        <button mat-raised-button color="primary" class="confirm-button" (click)="onYesClick()" cdkFocusInitial>Confirm</button>
      </mat-dialog-actions>
    </div>
  `,
  styles: [`
    :host {
      display: block;
    }

    .dialog-container {
      padding: 24px;
      background-color: #fff;
      border-radius: 12px;
      box-shadow: 0 8px 24px rgba(0, 0, 0, 0.1);
      min-width: 320px;
      max-width: 500px;
      font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
    }

    .dark .dialog-container {
      background-color: #1f2937;
      box-shadow: 0 8px 24px rgba(0, 0, 0, 0.3);
    }

    .dialog-title {
      margin: 0 0 20px 0;
      font-size: 22px;
      font-weight: 600;
      color: #003366; 
    }

    .dark .dialog-title {
      color: #60a5fa; 
    }

    .dialog-content {
      padding: 0;
      display: flex;
      flex-direction: column;
      gap: 20px;
    }

    .dialog-message {
      margin: 0;
      font-size: 16px;
      color: #333;
      line-height: 1.6;
    }

    .dark .dialog-message {
      color: #d1d5db; 
    }

    .comment-field {
      width: 100%;
      margin-top: 10px;
      background-color: #f9f9f9;
      border-radius: 6px;
    }

    .dark .comment-field {
      background-color: #374151; 
    }

    .comment-field .mat-form-field-fill {
      background-color: transparent;
    }

    .comment-field textarea {
      font-size: 14px;
      color: #333;
      line-height: 1.6;
      resize: vertical;
      min-height: 100px;
      padding: 10px;
      border: 1px solid #ddd;
      border-radius: 4px;
    }

    .dark .comment-field textarea {
      color: #d1d5db;
      border-color: #4b5563;
      background-color: #4b5563; 
    }

    .comment-field .mat-form-field-label {
      transform: translateY(1.5em) scale(1); 
      color: #666;
    }

    .comment-field .mat-form-field-can-float.mat-form-field-should-float .mat-form-field-label {
      transform: translateY(-1.5em) scale(0.75) !important;
      color: #003366;
    }

    .dark .comment-field .mat-form-field-label {
      color: #9ca3af; 
    }

    .dark .comment-field .mat-form-field-can-float.mat-form-field-should-float .mat-form-field-label {
      color: #60a5fa; 
    }

    .dialog-actions {
      display: flex;
      justify-content: flex-end;
      gap: 16px;
      padding-top: 20px;
      margin: 0;
    }

    .cancel-button {
      color: #666;
      font-weight: 500;
      text-transform: uppercase;
      padding: 8px 16px;
      border: 1px solid #ddd;
      border-radius: 6px;
      background-color: #fff;
    }

    .dark .cancel-button {
      color: #9ca3af;
      border-color: #4b5563;
      background-color: #1f2937;
    }

    .confirm-button {
      background-color: #003366 !important; 
      color: white !important;
      font-weight: 600;
      text-transform: uppercase;
      padding: 8px 20px;
      border-radius: 6px;
    }

    .dark .confirm-button {
      background-color: #60a5fa !important; 
    }

    .confirm-button:hover {
      background-color: #002244 !important; 
    }

    .dark .confirm-button:hover {
      background-color: #3b82f6 !important; 
    }
  `]
})
export class ConfirmDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { message: string; showComments?: boolean; comments?: string }
  ) {}

  onNoClick(): void {
    this.dialogRef.close(false);
  }

  onYesClick(): void {
    this.dialogRef.close({ confirmed: true, comments: this.data.comments || '' });
  }
}