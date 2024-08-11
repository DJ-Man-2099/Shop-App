import {
  HttpClient,
  HttpErrorResponse,
  HttpResponse,
} from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { firstValueFrom, of } from 'rxjs';
import { GroupInput, GroupOutput } from '../interfaces/group';
import { SimpleHttpClientService } from './simple-http-client.service';

@Injectable({
  providedIn: 'root',
})
export class GroupService {
  changeGroups = new EventEmitter<void>();

  constructor(private http: SimpleHttpClientService) {}

  async getAllGroups(): Promise<HttpResponse<Array<GroupOutput>>> {
    return firstValueFrom(this.http.get<Array<GroupOutput>>('api/Group'));
  }

  async AddNewGroup(group: GroupInput): Promise<HttpResponse<GroupOutput>> {
    return firstValueFrom(this.http.post<GroupOutput>('api/Group', group));
  }

  async EditGroup(
    id: number,
    group: GroupInput
  ): Promise<HttpResponse<GroupOutput>> {
    return firstValueFrom(
      this.http.patch<GroupOutput>(`api/Group/${id}`, group)
    );
  }
}
