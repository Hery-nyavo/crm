package site.easy.to.build.crm.service.api;

import site.easy.to.build.crm.exception.ApiException;

import java.util.HashMap;

public interface ApiLoginService {
    public HashMap<String, Object> login(HashMap<String, String> param) throws ApiException;
}
