import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { GroupOutput } from '../../interfaces/group';
import { GroupService } from '../../Services/group.service';
import { ModalNavigateService } from '../../Services/modal-navigate.service';
import { AddNewGroupComponent } from '../modal/add-new-group/add-new-group.component';
import { EditGroupComponent } from '../modal/edit-group/edit-group.component';
import { AuthenticationService } from '../../Services/authentication.service';

@Component({
  selector: 'app-groups-list',
  standalone: true,
  imports: [],
  templateUrl: './groups-list.component.html',
  styleUrl: './groups-list.component.css',
})
export class GroupsListComponent implements OnInit {
  static Path = 'groups-list';

  groups: GroupOutput[] = [];
  isEdible = false;

  constructor(
    private groupService: GroupService,
    private cdr: ChangeDetectorRef,
    private modal: ModalNavigateService,
    private authService: AuthenticationService
  ) {
    this.isEdible = this.authService.user?.role === 'Admin';
  }

  ngOnInit() {
    this.getAllGroups();
    this.groupService.changeGroups.subscribe(() => {
      this.getAllGroups();
    });
  }

  async getAllGroups() {
    const response = await this.groupService.getAllGroups();
    if (response.ok) {
      this.groups = response.body!;
      this.cdr.detectChanges();
    }
  }

  addNewGroup() {
    this.modal.goToModal([AddNewGroupComponent.Path]);
  }

  editGroup(group: GroupOutput) {
    this.modal.goToModal(
      [EditGroupComponent.Path, group.id!.toString()],
      group
    );
  }
}
