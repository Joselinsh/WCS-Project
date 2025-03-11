import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-success-popup',
  standalone: true, 
  imports: [MatDialogModule], 
  template: `
    <div class="success-popup">
      <div class="checkmark-circle">
        <svg class="checkmark-icon" viewBox="0 0 24 24">
          <path
            class="checkmark-path"
            d="M9 16.2L4.8 12l-1.4 1.4L9 19 21 7l-1.4-1.4L9 16.2z"
          />
        </svg>
      </div>
      <h2 class="success-message">{{ data.message }}</h2>
    </div>
  `,
  styles: [`
    .success-popup {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 20px;
      text-align: center;
      background-color: #003366; 
      color: white;
      border-radius: 8px;
      box-shadow: 0 3px 5px rgba(0, 0, 0, 0.2);
      min-width: 250px;
    }

    .checkmark-circle {
      width: 60px;
      height: 60px;
      border-radius: 50%;
      background-color: white;
      display: flex;
      align-items: center;
      justify-content: center;
      margin-bottom: 15px;
      animation: bounceIn 0.5s ease-out;
    }

    .checkmark-icon {
      width: 40px;
      height: 40px;
      fill: none;
      stroke: #007DB8; 
      stroke-width: 2;
      stroke-linecap: round;
      stroke-linejoin: round;
    }

    .checkmark-path {
      stroke-dasharray: 24;
      stroke-dashoffset: 24;
      animation: drawCheckmark 0.5s ease-in-out forwards 0.2s;
    }

    .success-message {
      font-size: 18px;
      font-weight: 500;
      margin: 0;
    }

    @media (prefers-color-scheme: dark) {
      .success-popup {
        background-color: #001F4D; 
      }

      .checkmark-circle {
        background-color: #E0E0E0; 
      }

      .checkmark-icon {
        stroke: #66B0FF; 
      }
    }

    @keyframes bounceIn {
      0% { transform: scale(0); }
      50% { transform: scale(1.2); }
      100% { transform: scale(1); }
    }

    @keyframes drawCheckmark {
      0% { stroke-dashoffset: 24; }
      100% { stroke-dashoffset: 0; }
    }
  `]
})
export class SuccessPopupComponent implements OnInit {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { message: string },
    private dialogRef: MatDialogRef<SuccessPopupComponent>
  ) {}

  ngOnInit(): void {
   
    setTimeout(() => {
      this.dialogRef.close();
    }, 3000);
  }
}