﻿import { Injectable, EventEmitter } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import './rxjs-operators';
import { LoggerService } from './logger.service';
import { IErrorMessage, ErrorMessage } from "../models/errorMessage";

@Injectable()
export class HttpErrorHandlerService {
    errorOccured = new EventEmitter();
    constructor(private _logger: LoggerService) { }

    responseError(errorResponse: Response | any) {
        let errMsg: IErrorMessage;
        if (errorResponse instanceof Response) {
            errMsg = new ErrorMessage(errorResponse.status, errorResponse.statusText);
        } else {
            errMsg = new ErrorMessage(errorResponse.status ? errorResponse.status : -1, errorResponse.message ? errorResponse.message : errorResponse.toString());
        }

        this.errorOccured.emit(errMsg);
        this._logger.error(`Response Error:`, errMsg);
        return Observable.throw(errMsg);
    }
}