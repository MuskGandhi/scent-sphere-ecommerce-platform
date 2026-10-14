import { Component, signal } from '@angular/core';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { FooterComponent } from './layouts/footer/footer.component';
import { HeaderComponent } from './layouts/header/header.component';
import { NotificationComponent } from './shared/components/notification/notification.component';

@Component({
  imports: [RouterOutlet, HeaderComponent, FooterComponent, RouterModule, NotificationComponent],
  standalone: true,
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  protected readonly title = signal('scentsphere-ui');

  constructor(public router: Router) {}

  showLayout(): boolean {
    return !['/login', '/register'].includes(this.router.url);
  }
}
