import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface NotificationMessage {
  message: string;
  type: 'success' | 'error' | 'info';
}

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private notificationSubject = new BehaviorSubject<NotificationMessage | null>(null);

  notification$ = this.notificationSubject.asObservable();
  success(message: string): void {
    this.notificationSubject.next({
      message,
      type: 'success',
    });

    setTimeout(() => {
      this.clear();
    }, 3000);
  }

  error(message: string): void {
    this.notificationSubject.next({
      message,
      type: 'error',
    });

    setTimeout(() => {
      this.clear();
    }, 3000);
  }

  info(message: string): void {
    this.notificationSubject.next({
      message,
      type: 'info',
    });

    setTimeout(() => {
      this.clear();
    }, 3000);
  }

  clear(): void {
    this.notificationSubject.next(null);
  }
}
