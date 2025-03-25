package site.easy.to.build.crm.service.api;

import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import site.easy.to.build.crm.entity.Role;
import site.easy.to.build.crm.entity.User;
import site.easy.to.build.crm.exception.ApiException;
import site.easy.to.build.crm.repository.UserRepository;
import site.easy.to.build.crm.util.ApiResponseUtil;
import site.easy.to.build.crm.util.EmailTokenUtils;

import java.util.HashMap;
import java.util.List;

@Service
public class ApiLoginServiceImpl implements ApiLoginService {
    private final PasswordEncoder passwordEncoder;
    private final UserRepository userRepository;

    public ApiLoginServiceImpl(PasswordEncoder passwordEncoder,
                               UserRepository userRepository) {
        this.passwordEncoder = passwordEncoder;
        this.userRepository = userRepository;
    }

    @Override
    public HashMap<String, Object> login(HashMap<String, String> param) throws ApiException {
        HashMap<String, Object> errors = new HashMap<>();
        if (!param.containsKey("username")) {
            errors.put("username", "username required");
        }
        if (!param.containsKey("password")) {
            errors.put("password", "password required");
        }
        String username = param.get("username");
        String password = param.get("password");

        List<User> users = userRepository.findByUsername(username);
        if (users.isEmpty()) {
            errors.put("username", "username not found");
        }
        User user = users.get(0);
        for (Role role : user.getRoles()) {
            if (role.getName().equals("ROLE_CUSTOMER")) {
                errors.put("username", "only customer and employee can login");
                break;
            }
        }

        if (!passwordEncoder.matches(password, user.getPassword())) {
            errors.put("password", "incorrect password");
        }

        if (!errors.isEmpty()) {
            throw new ApiException("", errors);
        }

        HashMap<String, Object> data = new HashMap<>();
        data.put("user", user);

        return data;
    }
}
