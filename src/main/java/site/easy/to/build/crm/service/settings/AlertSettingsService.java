package site.easy.to.build.crm.service.settings;

import site.easy.to.build.crm.entity.settings.AlertSettings;
import site.easy.to.build.crm.repository.settings.AlertSettingsRepository;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class AlertSettingsService {


    @Autowired
    AlertSettingsRepository alertRepo;

    public void Update(Double rate){

        if (rate < 0) {
            throw new NumberFormatException("Chiffre negatif interdit.");
        }
    AlertSettings alert=alertRepo.getSettings();
    alert.setRate(rate);
    alertRepo.save(alert);
    }

    public double getSettings(){
        return alertRepo.getSettings().getRate();
    }
    
}
