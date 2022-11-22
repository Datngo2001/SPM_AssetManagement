import { AxiosResponse } from "axios";
import qs from "qs";

import RequestService from "src/services/request";
import EndPoints from "src/constants/endpoints";
import IUserForm from "src/interfaces/User/IUserForm";
import IUser from "src/interfaces/User/IUser";
import IPagedModel from "src/interfaces/IPagedModel";

export function createUserRequest(
  userForm: IUserForm
): Promise<AxiosResponse<IUser>> {
  return RequestService.axios.post(EndPoints.user, userForm, {
    paramsSerializer: (params) => qs.stringify(params),
  });
}

export function updateUserRequest(
  userForm: IUserForm
): Promise<AxiosResponse<IUser>> {
  return RequestService.axios.put(EndPoints.user, userForm, {
    paramsSerializer: (params) => qs.stringify(params),
  });
}

export function getUsersRequest(
  userForm: IUserForm
): Promise<AxiosResponse<IPagedModel<IUser>>> {
  return RequestService.axios.put(EndPoints.user, userForm, {
    paramsSerializer: (params) => qs.stringify(params),
  });
}
