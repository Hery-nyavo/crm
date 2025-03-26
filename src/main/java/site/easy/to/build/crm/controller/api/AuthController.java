package site.easy.to.build.crm.controller.api;

import java.util.List;

import org.springframework.http.ResponseEntity;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import site.easy.to.build.crm.entity.LoginRequest;
import site.easy.to.build.crm.entity.LoginResponse;
import site.easy.to.build.crm.entity.User;
import site.easy.to.build.crm.service.user.UserService;

@RestController
@RequestMapping("/api/auth")
public class AuthController {

    private final UserService userService;

    public AuthController(UserService userService) {
        this.userService = userService;
    }

    @PostMapping("/login")
    public ResponseEntity<?> login(@RequestBody LoginRequest request) {

        User user=userService.findByEmail(request.getEmail());

        if (user!=null && request.getPassword().equals("Eval")) {
            String token = "GENERATED_JWT_TOKEN";

            LoginResponse response = new LoginResponse();
            response.setToken(token);
            response.setUsername("HeryAndriamandroso");
            response.setRoles(List.of("ROLE_MANAGER"));
    
            return ResponseEntity.ok(response);
        }
        return null;
       
    }
}
