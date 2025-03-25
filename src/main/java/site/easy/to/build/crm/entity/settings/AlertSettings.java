package site.easy.to.build.crm.entity.settings;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.math.BigDecimal;

@Entity
@Table(name = "alert_settings")
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class AlertSettings {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "alert_id")
    private int alertSettingsId;

    @Column(name = "rate", precision = 5, scale = 2)
    private Double rate;

 
}
