export * from './groups.service';
import { GroupsService } from './groups.service';
export * from './toDoBackend.service';
import { ToDoBackendService } from './toDoBackend.service';
export * from './toDos.service';
import { ToDosService } from './toDos.service';
export * from './users.service';
import { UsersService } from './users.service';
export const APIS = [GroupsService, ToDoBackendService, ToDosService, UsersService];
