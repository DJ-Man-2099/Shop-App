import { ChangeDetectorRef, Component } from '@angular/core';
import { GroupOutput } from '../../interfaces/group';
import { GroupService } from '../../Services/group.service';
import { ModalNavigateService } from '../../Services/modal-navigate.service';
import { AddNewGroupComponent } from '../modal/add-new-group/add-new-group.component';
import { EditGroupComponent } from '../modal/edit-group/edit-group.component';

@Component({
  selector: 'app-groups-list',
  standalone: true,
  imports: [],
  templateUrl: './groups-list.component.html',
  styleUrl: './groups-list.component.css',
})
export class GroupsListComponent {
  static Path = 'groups-list';

  groups: GroupOutput[] = [];

  constructor(
    private groupService: GroupService,
    private cdr: ChangeDetectorRef,
    private modal: ModalNavigateService
  ) {}

  ngOnInit() {
    this.getAllCategories();
    this.groupService.changeGroups.subscribe(() => {
      this.getAllCategories();
    });
  }

  async getAllCategories() {
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
