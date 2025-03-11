import { ApplicationConfig } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { routes } from './app.routes';
import { provideAnimations } from '@angular/platform-browser/animations';
import { MAT_SNACK_BAR_DEFAULT_OPTIONS } from '@angular/material/snack-bar';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async'; // For animations
import { MatDialogModule } from '@angular/material/dialog'; // Import for standalone use
import { MatButtonModule } from '@angular/material/button'; // Import for standalone use
import { NgChartsModule } from 'ng2-charts'; // Import for standalone use
import { MatSnackBarModule } from '@angular/material/snack-bar';
export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes, withComponentInputBinding()),
    MatSnackBarModule,
    provideAnimations(), // Already included, kept for consistency
    provideAnimationsAsync(), // Optional: Adds async animation support
    provideHttpClient(), // Already included, kept for consistency
    {
      provide: MAT_SNACK_BAR_DEFAULT_OPTIONS,
      useValue: {
        duration: 3000,
        horizontalPosition: 'center', // Match SnackbarService
        verticalPosition: 'top' // Match SnackbarService
      }
    },
    // Provide Angular Material modules (workaround for standalone)
    { provide: MatDialogModule, useValue: MatDialogModule }, // Temporary provider
    { provide: MatButtonModule, useValue: MatButtonModule }, // Temporary provider
    { provide: NgChartsModule, useValue: NgChartsModule } // Temporary provider
  ]
};