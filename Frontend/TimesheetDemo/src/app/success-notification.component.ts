import { Component, Inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA } from '@angular/material/snack-bar';

@Component({
  selector: 'app-success-notification',
  template: `
    <div class="success-container">
      <div class="checkmark-circle">
        <div class="checkmark">
          <svg class="checkmark-icon" viewBox="0 0 24 24">
            <path
              class="checkmark-path"
              d="M9 16.2L4.8 12l-1.4 1.4L9 19 21 7l-1.4-1.4L9 16.2z"
            />
          </svg>
        </div>
      </div>
      <div class="success-message">{{ data.message }}</div>
    </div>
  `,
  styles: [`
    .success-container {
      display: flex;
      align-items: center;
      justify-content: center;
      background-color: #4caf50;
      color: white;
      padding: 16px 24px;
      border-radius: 8px;
      box-shadow: 0 3px 5px rgba(0, 0, 0, 0.2);
    }
    .checkmark-circle {
      width: 40px;
      height: 40px;
      border-radius: 50%;
      background-color: white;
      display: flex;
      align-items: center;
      justify-content: center;
      margin-right: 12px;
      animation: bounceIn 0.5s ease-out;
    }
    .checkmark {
      width: 24px;
      height: 24px;
      position: relative;
    }
    .checkmark-icon {
      fill: none;
      stroke: #4caf50;
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
      font-size: 16px;
      font-weight: 500;
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
export class SuccessNotificationComponent {
  constructor(@Inject(MAT_SNACK_BAR_DATA) public data: { message: string }) {}
}