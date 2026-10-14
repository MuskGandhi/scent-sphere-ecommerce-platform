import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';

import {
  NotificationService,
  NotificationMessage,
} from '../../../core/services/notification.service';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.scss'],
})
export class NotificationComponent {
  notification$: Observable<NotificationMessage | null>;

  constructor(private notificationService: NotificationService) {
    this.notification$ = this.notificationService.notification$;
  }
}
