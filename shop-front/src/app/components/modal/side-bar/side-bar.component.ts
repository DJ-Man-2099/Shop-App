import { Component } from '@angular/core';
import { ModalNavigateService } from '../../../Services/modal-navigate.service';
import { CategoriesListComponent } from '../../categories-list/categories-list.component';
import { AppComponent } from '../../../app.component';
import { Router, RouterModule } from '@angular/router';
import { AuthenticationService } from '../../../Services/authentication.service';
import { GroupsListComponent } from '../../groups-list/groups-list.component';

@Component({
  selector: 'app-side-bar',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './side-bar.component.html',
  styleUrl: './side-bar.component.css',
})
export class SideBarComponent {
  static Path = 'side-bar';

  links = [
    { name: 'الصفحة الرئيسية', path: '' },
    { name: 'العيارات', path: CategoriesListComponent.Path },
    { name: 'المجموعات', path: GroupsListComponent.Path },
  ];

  constructor(
    private modal: ModalNavigateService,
    private router: Router,
    private authService: AuthenticationService
  ) {}

  dismiss() {
    this.modal.dismiss();
  }

  logout() {
    this.dismiss();
    this.authService.clearToken();
    this.router.navigate([{ outlets: { primary: ['login'], modal: null } }]);
  }

  goto(path: string) {
    this.modal.dismiss();
    if (path === '') {
      this.router.navigate([{ outlets: { primary: null, modal: null } }]);
    } else {
      this.router.navigate([{ outlets: { primary: [path], modal: null } }], {
        replaceUrl: true,
      });
    }
  }
}
