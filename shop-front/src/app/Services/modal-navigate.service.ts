import { EventEmitter, Injectable } from '@angular/core';
import { ActivatedRoute, NavigationExtras, Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class ModalNavigateService {
  toggle = new EventEmitter<boolean>();
  constructor(private router: Router, private currentRoute: ActivatedRoute) {}

  goToModal(path: string | string[], data?: any) {
    let extras: NavigationExtras | undefined;
    if (data) {
      extras = {
        state: { data },
      };
    }
    this.router.navigate([{ outlets: { modal: path } }], extras);
    this.toggle.emit(true);
  }

  dismiss() {
    this.router.navigate([{ outlets: { modal: null } }]);
    this.toggle.emit(false);
  }
}
